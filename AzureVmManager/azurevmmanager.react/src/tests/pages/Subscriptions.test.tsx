import { useEffect } from 'react';
import { act, render, waitFor, renderHook } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom'
import { MsalReactTester } from 'msal-react-tester';
import { MsalProvider } from '@azure/msal-react';
import Subscriptions from '../../pages/Subscriptions';
import Subscription from '../../dataObjects/Subscription';

let msalTester: MsalReactTester;

jest.mock('../../services/DataService', () => ({
    getSubscriptions: jest.fn(() => {
        const subscriptions: Subscription[] = [{
            id: "12345",
            name: "test 1"
        },
        {
            id: "5432",
            name: "test 2"
        }];
        return subscriptions;
    })
}));

jest.mock("react", () => ({
    ...jest.requireActual("react"),
    useEffect: jest.fn(),
  }));

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

test('Subscriptions component is loading', async () => {    
    await msalTester.isLogged();
    const { queryByText } = render(
        <MsalProvider instance={msalTester.client}>
            <MemoryRouter>
                <Subscriptions />
            </MemoryRouter>
        </MsalProvider>);

    expect(queryByText("Loading...")).toBeInTheDocument();
    expect(queryByText("12345")).not.toBeInTheDocument();
    expect(queryByText("54321")).not.toBeInTheDocument();
});

test('Subscriptions component loads subscriptions', async () => {    
    await msalTester.isLogged();
    const { queryByText } = render(
        <MsalProvider instance={msalTester.client}>
            <MemoryRouter>
                <Subscriptions />
            </MemoryRouter>
        </MsalProvider>);

    waitFor(() => {
        expect(queryByText("12345")).toBeInTheDocument();
        expect(queryByText("54321")).toBeInTheDocument();
        expect(queryByText("Loading...")).not.toBeInTheDocument();
    });
});