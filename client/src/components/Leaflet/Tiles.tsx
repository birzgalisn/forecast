'use client';

import { type WeatherGradients } from '@/components/Controls/Legend/gradients';
import { useSearchParams } from 'next/navigation';
import { Browser } from 'leaflet';
import { TileLayer } from 'react-leaflet';

type Tiles = {
  map: 'light_all' | 'rastertiles/voyager' | 'dark_all';
  weather: keyof WeatherGradients;
};

export default function Tiles() {
  const searchParams = useSearchParams();
  const map = searchParams.get('map') || 'rastertiles/voyager';
  const weather = searchParams.get('weather') || '';

  return (
    <>
      <TileLayer
        url={`https://{s}.basemaps.cartocdn.com/${map}/{z}/{x}/{y}${
          Browser.retina ? '@2x.png' : '.png'
        }`}
        attribution={`&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, &copy; <a href="https://carto.com/attributions">Carto</a>, &copy; <a href="https://openweathermap.org">Weather data provided by OpenWeather</a>`}
      />
      {!!weather && (
        <TileLayer
          url={`https://tile.openweathermap.org/map/${weather}/{z}/{x}/{y}.png?appid=${process.env.NEXT_PUBLIC_OPEN_WEATHER_API_KEY}`}
        />
      )}
    </>
  );
}
