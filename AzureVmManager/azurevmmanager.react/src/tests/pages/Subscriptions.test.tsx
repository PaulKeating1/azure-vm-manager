import { render, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event'
import { createMemoryRouter, MemoryRouter, RouterProvider } from 'react-router-dom'
import { MsalReactTester } from 'msal-react-tester';
import { MsalProvider } from '@azure/msal-react';
import Subscriptions from '../../pages/Subscriptions';
import Subscription from '../../dataObjects/Subscription';
import routesConfig from '../../routesConfig';

let msalTester: MsalReactTester;

jest.mock('../../services/DataService', () => {
    return function() {
        return {    
            getSubscriptions: jest.fn(() => {
            const subscriptions: Subscription[] = [{
                id: "12345",
                name: "test 1"
            },
            {
                id: "54321",
                name: "test 2"
            }];
            return subscriptions;
        })};
    }
});

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

test('Subscriptions component loads subscriptions', async () => {    
    await msalTester.isLogged();
    const { getByText, queryByText } = render(
        <MsalProvider instance={msalTester.client}>
            <MemoryRouter>
                <Subscriptions />
            </MemoryRouter>
        </MsalProvider>);

    expect(getByText("Loading...")).toBeInTheDocument();

    await waitFor(() => getByText("12345"));

    expect(getByText("12345")).toBeInTheDocument();
    expect(getByText("54321")).toBeInTheDocument();
    expect(queryByText("Loading...")).not.toBeInTheDocument();
});

test('Clicking on subscription loads resource groups for the subscription', async () => {
    await msalTester.isLogged();
    const user = userEvent.setup()
    const router = createMemoryRouter(routesConfig, {
        initialEntries: ["/subscriptions"]
      });

    const { getByText } = render(
        <MsalProvider instance={msalTester.client}>
            <RouterProvider router={router} />
        </MsalProvider>);

    await waitFor(() => getByText("test 1"));

    const subscription = getByText("test 1");
    user.click(subscription);

    await waitFor(() => {
        expect(router.state.location.pathname).toEqual('/resource-groups/12345');
    });
});