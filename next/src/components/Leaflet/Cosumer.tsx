'use client';

import { useMemo } from 'react';
import { type LeafletEventHandlerFnMap } from 'leaflet';
import { useMapEvents } from 'react-leaflet/hooks';
import usePositionAtom from '@/components/Leaflet/Atoms/position';

export default function Cosumer() {
  const [_position, setPosition] = usePositionAtom();

  const handlers = useMemo<LeafletEventHandlerFnMap>(
    () => ({
      click: (e) => {
        setPosition(e.latlng);
      },
    }),
    [setPosition],
  );

  useMapEvents(handlers);

  return null;
}
