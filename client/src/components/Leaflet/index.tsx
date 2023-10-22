'use client';

import dynamic from 'next/dynamic';
import { MapContainer } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';

import Tiles from '@/components/Leaflet/Tiles';
import Cosumer from '@/components/Leaflet/Cosumer';
import usePositionAtom from '@/components/Leaflet/Atoms/position';
import Report from '@/components/Report';

const LazyCreateMarker = dynamic(() => import('../Report/CreateMarker'));

export default function Leaflet() {
  const [position, setPosition] = usePositionAtom();

  return (
    <MapContainer
      className="h-screen w-screen"
      center={[56.6511, 23.7196]}
      zoom={4}
      zoomControl={false}
    >
      <Tiles />
      <Cosumer />
      <Report />
      {!!position && (
        <LazyCreateMarker position={position} setPosition={setPosition} />
      )}
    </MapContainer>
  );
}
