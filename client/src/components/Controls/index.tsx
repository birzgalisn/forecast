'use client';

import dynamic from 'next/dynamic';
import { useSearchParams, useRouter } from 'next/navigation';
import { type Legend } from '@/components/Controls/Legend';

const LazyLegend = dynamic(() => import('@/components/Controls/Legend'), {
  loading: (_props) => (
    <p className="flex items-center justify-center text-base">
      Loading legend...
    </p>
  ),
});

export default function Controls() {
  const router = useRouter();
  const searchParams = useSearchParams();

  const map = searchParams.get('map') || 'rastertiles/voyager';
  const weather = (searchParams.get('weather') || '') as Legend['active'];

  const handleOptionsChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    event.preventDefault();

    const { name, value } = event.target;

    const newParams = new URLSearchParams(`${searchParams}`);

    if (value) {
      newParams.set(name, value);
    } else {
      newParams.delete(name);
    }

    router.push(`?${newParams}`);
  };

  return (
    <div className="absolute flex w-full justify-end">
      <div className="z-[500] m-3 flex w-full max-w-lg flex-col gap-2 rounded border bg-white p-2">
        <select
          className="flex w-full rounded-sm border bg-white p-[0.125rem] pl-2 text-base"
          name="map"
          value={map}
          onChange={handleOptionsChange}
        >
          <option value="light_all">Light</option>
          <option value="rastertiles/voyager">Voyager</option>
          <option value="dark_all">Dark</option>
        </select>
        <select
          className="flex w-full rounded-sm border bg-white p-[0.125rem] pl-2 text-base"
          name="weather"
          value={weather}
          onChange={handleOptionsChange}
        >
          <option value="">None</option>
          <option value="clouds_new">Clouds</option>
          <option value="precipitation_new">Precipitation</option>
          <option value="pressure_new">Pressure</option>
          <option value="wind_new">Wind speed</option>
          <option value="temp_new">Temperature</option>
        </select>
        {!!weather && <LazyLegend active={weather} />}
      </div>
    </div>
  );
}
