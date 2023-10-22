import { type LatLng } from 'leaflet';
import { atom, useAtom } from 'jotai';

export type Position = LatLng;

const positionAtom = atom<Position | null>(null);

function usePositionAtom() {
  return useAtom(positionAtom);
}

export default usePositionAtom;
