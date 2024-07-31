import { render, screen } from '@testing-library/react';
import SiteHeader from '../../components/SiteHeader';
import {BrowserRouter, MemoryRouter} from 'react-router-dom'

test('SiteHeader component has site name', () => {
    const { getByText } = render(<SiteHeader />, {wrapper: BrowserRouter});
    expect(getByText("Azure VM Manager")).toBeInTheDocument();
    expect(getByText("Azure VM Manager")).toHaveClass("site-header");
  });

test('SiteHeader nav has menu items', () => {
    const { getByText } = render(<SiteHeader />, {wrapper: BrowserRouter});

    const home = getByText("Home");
    expect(home).toBeInTheDocument();
    expect(home).toHaveClass("site-header-menu-item");
  });