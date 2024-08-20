import { render, waitFor } from '@testing-library/react';
import SiteHeader from '../../components/SiteHeader';
import { BrowserRouter, MemoryRouter } from 'react-router-dom'
import { MsalReactTester } from 'msal-react-tester';
import { MsalProvider } from '@azure/msal-react';

let msalTester: MsalReactTester;

beforeEach(() => {
    // new instance of msal tester for each test:
    msalTester = new MsalReactTester();
    // or new MsalReactTester("Redirect") / new MsalReactTester("Popup")

    // Ask msal-react-tester to handle and mock all msal-react processes:
    msalTester.spyMsal();
});

afterEach(() => {
    // reset msal-react-tester
    msalTester.resetSpyMsal();
});

test('SiteHeader component has site name', () => {
    const { getByText } = render(<SiteHeader />, { wrapper: BrowserRouter });
    expect(getByText("Azure VM Manager")).toBeInTheDocument();
});

test('SiteHeader nav has menu items for unauthenticated user', async () => {
    await msalTester.isNotLogged();
    const { getByText } = render(<SiteHeader />, { wrapper: BrowserRouter });

    expect(getByText("Home")).toBeInTheDocument();
    expect(getByText("Login")).toBeInTheDocument();
});

test('SiteHeader nav has menu items for authenticated user', async () => {
    await msalTester.isLogged();
    const { getByText } = await waitFor(() => render(
        <MsalProvider instance={msalTester.client}>
            <MemoryRouter>
                <SiteHeader />
            </MemoryRouter>
        </MsalProvider>));

    await msalTester.waitForRedirect();
    expect(getByText("Home")).toBeInTheDocument();
    expect(getByText("Logout")).toBeInTheDocument();
});