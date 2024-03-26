import { atom, useAtom } from 'jotai';

export type WeatherForecast = {
  id: number;
  location: string;
  lat: number;
  lng: number;
  temperatureC: number;
  createdAt: string;
  updatedAt?: string;
};

const weatherForecastsAtom = atom<WeatherForecast[]>([]);

function useWeatherForecastsAtom() {
  return useAtom(weatherForecastsAtom);
}

export default useWeatherForecastsAtom;
