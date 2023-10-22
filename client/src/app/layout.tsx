import './globals.css';
import type { Metadata } from 'next';

export const metadata: Metadata = {
  title: 'Forecast - Your global weather app',
  description:
    'Explore weather conditions worldwide with our interactive weather application. Get real-time data on clouds, precipitation, pressure, wind speed, and temperature. Plan your activities with accurate weather forecasts.',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
