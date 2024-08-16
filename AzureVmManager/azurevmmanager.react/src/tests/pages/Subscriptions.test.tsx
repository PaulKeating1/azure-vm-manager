import { render, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event'
import { createMemoryRouter, MemoryRouter, RouterProvider } from 'react-router-dom'
import { MsalReactTester } from 'msal-react-tester';
import { MsalProvider } from '@azure/msal-react';
import Subscriptions from '../../pages/Subscriptions';
import Subscription from '../../dataObjects/Subscription';
import routesConfig from '../../routesConfig';

let msalTester: MsalReactTester;

const mockGetSubscriptions = jest.fn();

jest.mock('../../services/DataService', () => {
    return function() {
        return {
            getSubscriptions: mockGetSubscriptions,
            getResourceGroups: jest.fn(() => {})
        };        
    };
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

function getSubscriptionsSuccess() {
    const subscriptions: Subscription[] = [{
        id: "12345",
        name: "test 1"
    },
    {
        id: "54321",
        name: "test 2"
    }];
    return subscriptions;
}

test('Subscriptions component loads subscriptions', async () => {    
    mockGetSubscriptions.mockImplementationOnce(getSubscriptionsSuccess);    
    await msalTester.isLogged();
    const { getByText, queryByText } = render(
        <MsalProvider instance={msalTester.client}>
            <MemoryRouter>
                <Subscriptions />
            </MemoryRouter>
        </MsalProvider>);

    expect(getByText("Loading...")).toBeInTheDocument();

    await waitFor(() => getByText("12345"));

    expect(getByText("test 1")).toBeInTheDocument();
    expect(getByText("12345")).toBeInTheDocument();
    expect(getByText("test 2")).toBeInTheDocument();
    expect(getByText("54321")).toBeInTheDocument();
    expect(queryByText("Loading...")).not.toBeInTheDocument();
});

test('Clicking on subscription loads resource groups for the subscription', async () => {
    mockGetSubscriptions.mockImplementationOnce(getSubscriptionsSuccess);    
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

test("Error getting subscriptions is handled and error alert displayed", async () => {
    const apiError = "API error!";
    mockGetSubscriptions.mockRejectedValueOnce(new Error(apiError));    
    await msalTester.isLogged();
    const { getByText, getByRole } = render(
        <MsalProvider instance={msalTester.client}>
            <MemoryRouter>
                <Subscriptions />
            </MemoryRouter>
        </MsalProvider>);

    expect(getByText("Loading...")).toBeInTheDocument();

    await waitFor(() => getByRole("alert"));

    expect(getByRole("alert").querySelector("div.MuiAlert-message")?.textContent).toContain(apiError);
});