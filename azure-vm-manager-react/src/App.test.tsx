import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';

jest.mock('./SiteHeader', () => {
  const SiteHeader = () => <div data-testid="SiteHeader">SiteHeader</div>;
  return SiteHeader;
});

test('App renders site header component', () => {
  const { getByTestId } = render(<App />);
  expect(getByTestId("SiteHeader")).toBeInTheDocument();
});
