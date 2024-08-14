import { render, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom'
import { MsalReactTester } from 'msal-react-tester';
import { MsalProvider } from '@azure/msal-react';
import ResourceGroups from '../../pages/ResourceGroups';
import ResourceGroup from '../../dataObjects/ResourceGroup';

let msalTester: MsalReactTester;

jest.mock('../../services/DataService', () => {
    return function() {
        return {    
            getResourceGroups: jest.fn(() => {
            const resourceGroups: ResourceGroup[] = [{
                id: "12345",
                name: "rg-test-1",
                location: "uksouth",
                subscriptionName: "Test subscription"
            },
            {
                id: "54321",
                name: "rg-test-2",
                location: "uksouth",
                subscriptionName: "Test subscription"
            }];
            return resourceGroups;
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

test('ResourceGroups component loads resource groups', async () => {    
    await msalTester.isLogged();
    const { getByText, queryByText, getAllByText } = render(
        <MsalProvider instance={msalTester.client}>
            <MemoryRouter>
                <ResourceGroups />
            </MemoryRouter>
        </MsalProvider>);

    expect(getByText("Loading...")).toBeInTheDocument();

    await waitFor(() => getByText("rg-test-1"));

    expect(getByText("rg-test-1")).toBeInTheDocument();
    expect(getByText("rg-test-2")).toBeInTheDocument();
    expect(getAllByText("uksouth").length).toBe(2);
    expect(getAllByText("Test subscription").length).toBe(2);
    expect(queryByText("Loading...")).not.toBeInTheDocument();
});