import weatherGradients, {
  type WeatherGradients,
} from '@/components/Controls/Legend/gradients';

export type Legend = {
  active: keyof WeatherGradients;
};

export default function Legend({ active }: Legend) {
  const gradient = weatherGradients[active];

  if (!gradient) {
    return null;
  }

  return (
    <div className="flex rounded border bg-white p-2">
      <div className="mr-6 flex items-center justify-start whitespace-nowrap text-base">
        {gradient.title}
      </div>
      <div className="flex-grow">
        <div className="flex items-center justify-between">
          {gradient.markers.map((marker, index) => (
            <div key={index} className="text-center">
              <div className="text-xs">{marker}</div>
            </div>
          ))}
        </div>
        <div className="h-1 rounded-sm" style={gradient.gradient} />
      </div>
    </div>
  );
}
