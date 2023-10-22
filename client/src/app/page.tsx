'use client';

import dynamic from 'next/dynamic';
import Controls from '@/components/Controls';

const LazyLeaflet = dynamic(() => import('@/components/Leaflet'), {
  ssr: false,
  loading: (_props) => (
    <p className="absolute z-[600] flex h-screen w-screen items-center justify-center bg-white text-base">
      Loading the map...
    </p>
  ),
});

export default function Home() {
  return (
    <main>
      <Controls />
      <LazyLeaflet />
    </main>
  );
}
