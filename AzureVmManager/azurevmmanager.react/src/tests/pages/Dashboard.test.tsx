import { render, waitFor } from '@testing-library/react';
import Dashboard from '../../pages/Dashboard';
import { MemoryRouter } from 'react-router-dom'
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

test('Dashboard component has text', async () => {
    await msalTester.isLogged();
    const { getByText } = await waitFor(() => render(
        <MsalProvider instance={msalTester.client}>
            <MemoryRouter>
                <Dashboard />
            </MemoryRouter>
        </MsalProvider>));

    await msalTester.waitForRedirect();
    expect(getByText("Dashboard")).toBeInTheDocument();
});