import { Popup } from 'react-leaflet';
import { type Position } from '@/components/Leaflet/Atoms/position';
import { useRef } from 'react';

type CreateMarker = {
  position: Position;
  setPosition: (position: Position | null) => void;
};

export default function CreateMarker({ position, setPosition }: CreateMarker) {
  const locationRef = useRef<HTMLInputElement>(null);
  const temperatureCRef = useRef<HTMLInputElement>(null);
  const buttonRef = useRef<HTMLButtonElement>(null);

  const handleClose = (event?: React.SyntheticEvent<HTMLAnchorElement>) => {
    event?.stopPropagation();

    if (locationRef.current) {
      locationRef.current.value = '';
    }

    if (temperatureCRef.current) {
      temperatureCRef.current.value = '';
    }

    setPosition(null);
  };

  const handleSubmit = async (event: React.SyntheticEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (buttonRef.current) {
      buttonRef.current.textContent = 'Saving...';
    }

    await fetch(`${process.env.NEXT_PUBLIC_API_URL}/weatherforecast`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        location: locationRef.current?.value,
        temperatureC: temperatureCRef.current?.value,
        ...position,
      }),
    })
      .catch(console.error)
      .finally(handleClose);
  };

  return (
    <Popup
      position={[position.lat, position.lng]}
      autoClose={false}
      closeOnClick={false}
      closeButton={false}
    >
      <a
        className="leaflet-popup-close-button"
        role="button"
        aria-label="Close popup"
        onClick={handleClose}
      >
        <span aria-hidden="true">{'\u00D7'}</span>
      </a>
      <form className="w-72" onSubmit={handleSubmit}>
        <h1 className="mb-3 text-lg">New weather report</h1>
        <div className="mb-2 flex w-full justify-between">
          <label className="flex w-1/4 items-center pr-2" htmlFor="location">
            Location
          </label>
          <input
            className="w-3/5 rounded-sm border p-[0.125rem] pl-2"
            ref={locationRef}
            id="location"
            required
          />
        </div>
        <div className="mb-2 flex w-full justify-between">
          <label
            className="flex w-1/4 items-center whitespace-nowrap pr-2"
            htmlFor="temperatureC"
          >
            Temperature, {'\u00B0'}C
          </label>
          <input
            className="w-3/5 rounded-sm border p-[0.125rem] pl-2"
            ref={temperatureCRef}
            id="temperatureC"
            required
          />
        </div>
        <div className="mb-2 flex w-full justify-between">
          <label className="flex w-1/4 items-center pr-2" htmlFor="lat">
            Latitude
          </label>
          <input
            className="w-3/5 rounded-sm border border-transparent p-[0.125rem] pl-2"
            id="lat"
            value={position.lat.toFixed(3)}
            disabled
          />
        </div>
        <div className="flex w-full justify-between">
          <label className="flex w-1/4 items-center pr-2" htmlFor="lng">
            Longitude
          </label>
          <input
            className="w-3/5 rounded-sm border border-transparent p-[0.125rem] pl-2"
            id="lng"
            value={position.lng.toFixed(3)}
            disabled
          />
        </div>
        <button
          className="mt-2 w-full rounded-sm border px-3 py-1"
          ref={buttonRef}
          type="submit"
        >
          Report
        </button>
      </form>
    </Popup>
  );
}
