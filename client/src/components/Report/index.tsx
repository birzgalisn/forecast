'use client';

import { useEffect } from 'react';
import MarkerClusterGroup from 'react-leaflet-cluster';
import useWeatherForecastsAtom, {
  type WeatherForecast,
} from '@/components/Leaflet/Atoms/weatherForecasts';
import WeatherMarker from '@/components/Report/WeatherMarker';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export default function Report() {
  const [weatherForecasts, setWeatherForecasts] = useWeatherForecastsAtom();

  useEffect(() => {
    const openSingalRConnection = async () => {
      const connection = new HubConnectionBuilder()
        .withUrl(`${process.env.NEXT_PUBLIC_API_URL}/hub`)
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build();

      connection.on('NewForecast', (newWeatherForecast: WeatherForecast) => {
        setWeatherForecasts((prev) => {
          if (prev.some((f) => f.id === newWeatherForecast.id)) {
            return prev;
          }
          return prev.concat(newWeatherForecast);
        });
      });

      connection.on(
        'FahrenheitCalculated',
        (forecastId: number, averageTemperatureF: number) => {
          setWeatherForecasts((prev) =>
            prev.map((wf) => {
              if (wf.id === forecastId) {
                return { ...wf, averageTemperatureF };
              }
              return wf;
            }),
          );
        },
      );

      await connection.start();
    };

    const fetchWeatherForecasts = async () => {
      const res = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/weatherforecast`,
      );

      const wf: WeatherForecast[] = await res.json();

      setWeatherForecasts(wf);
    };

    fetchWeatherForecasts().then(openSingalRConnection).catch(console.error);
  }, [setWeatherForecasts]);

  return (
    <MarkerClusterGroup>
      {weatherForecasts.map((wf) => (
        <WeatherMarker key={wf.id} weatherForecast={wf} />
      ))}
    </MarkerClusterGroup>
  );
}
