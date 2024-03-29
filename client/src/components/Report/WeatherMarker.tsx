import { type WeatherForecast } from '@/components/Leaflet/Atoms/weatherForecasts';
import { memo } from 'react';
import { Icon } from 'leaflet';
import { Marker, Popup } from 'react-leaflet';
import { format } from 'date-fns';

type WeatherMarker = {
  weatherForecast: WeatherForecast;
};

function WeatherMarker({ weatherForecast }: WeatherMarker) {
  return (
    <Marker
      icon={
        new Icon({
          iconUrl: `${process.env.NEXT_PUBLIC_BASE_URL}/marker-icon.png`,
          iconAnchor: [12.5, 44],
        })
      }
      position={[weatherForecast.lat, weatherForecast.lng]}
    >
      <Popup>
        <div className="w-72">
          <h2 className="mb-3 text-lg">{weatherForecast.location}</h2>
          <div className="mb-2 flex w-full justify-between">
            <p className="!m-0 flex w-1/2 items-center whitespace-nowrap !p-0">
              Temperature, {'\u00B0'}C
            </p>
            <p className="!m-0 w-1/2 !p-0">{weatherForecast.temperatureC}</p>
          </div>
          <div className="mb-2 flex w-full justify-between">
            <p className="!m-0 flex w-1/2 items-center whitespace-nowrap !p-0">
              Forecasted at
            </p>
            <p className="!m-0 w-1/2 !p-0">
              {format(new Date(weatherForecast.createdAt), 'p PP')}
            </p>
          </div>
        </div>
      </Popup>
    </Marker>
  );
}

export default memo(
  WeatherMarker,
  (prev, next) => prev.weatherForecast.id === next.weatherForecast.id,
);
