import { MsalReactTester } from "msal-react-tester";
import DataService from "../../services/DataService";
import { AccountInfo, AuthenticationResult } from "@azure/msal-browser";

const responseMock = new Response();
const url = "test";
const accessToken = "1234567890";
const msalTester = new MsalReactTester();
const accountInfo: AccountInfo = {
    homeAccountId: "",
    environment: "",
    tenantId: "",
    username: "",
    localAccountId: ""
};

const mockGetOk = jest.fn();

beforeEach(() => {
    global.fetch = jest.fn((requestUrl, options) =>{
        Object.defineProperty(responseMock, 'url', {
            get: () => requestUrl
        });
        Object.defineProperty(responseMock, 'headers', {
            get: () => options.headers
        });
        Object.defineProperty(responseMock, 'ok', {
            get: mockGetOk
        });
        return Promise.resolve<Response>(responseMock);
    }) as jest.Mock;    

    const authResult = Promise.resolve<AuthenticationResult>({
        authority: "", 
        uniqueId: "", 
        tenantId: "", 
        scopes: [""], 
        account: accountInfo, 
        idToken: "", 
        idTokenClaims: {}, 
        accessToken: accessToken, 
        fromCache: false, 
        expiresOn: null, 
        extExpiresOn: undefined,
        tokenType: "",
        correlationId: ""
    });
    msalTester.client.acquireTokenSilent = jest.fn(() => authResult);
});

test("Get method creates fetch request with correctly formatted URL and access token in authorization header", async () => {
    const dataService = new DataService(msalTester.client, accountInfo);
    mockGetOk.mockImplementationOnce(() => true);
    const fetchResponse = await dataService.get(url);
    const headers = (fetchResponse.headers as unknown) as [[]];
    const firstHeader = headers[0] as Array<string>;
    const firstHeaderKey = firstHeader.length > 0 ? firstHeader[0] : null;
    const firstHeaderValue = firstHeader.length > 1 ? firstHeader[1] : null;

    expect(fetchResponse.url).toBe(`${window.location.origin}/api/${url}`);
    expect(firstHeaderKey).toBe("Authorization");
    expect(firstHeaderValue).toBe(`Bearer ${accessToken}`);
});

test("Get method throws error when fetch request is unsuccessful", async () => {
    const dataService = new DataService(msalTester.client, accountInfo);
    mockGetOk.mockImplementationOnce(() => false);
    const errorMessage = "API error!";
    responseMock.text = jest.fn(() => errorMessage) as jest.Mock;
    let fetchResponse;
    let errorThrown;

    try {
        fetchResponse = await dataService.get(url);
    } catch (error) {
        errorThrown = error;
    }

    expect(fetchResponse).toBeFalsy();
    expect(errorThrown instanceof Error).toBeTruthy();
    errorThrown = errorThrown as Error;
    expect(errorThrown.message).toBe(errorMessage);
});

test("GetSubscriptions method returns subscriptions", async () => {
    const dataService = new DataService(msalTester.client, accountInfo);
    mockGetOk.mockImplementationOnce(() => true);
    responseMock.json = jest.fn(() => [{
        id: "subscription-id-1",
        name: "subscription-name-1"
    },
    {
        id: "subscription-id-2",
        name: "subscription-name-2"
    }]) as jest.Mock;
    const subscriptions = await dataService.getSubscriptions(url);

    expect(subscriptions.length).toBe(2);
    expect(subscriptions[0].id).toBe("subscription-id-1");
    expect(subscriptions[1].id).toBe("subscription-id-2");
});

test("GetResourceGroups method returns resource groups", async () => {
    const dataService = new DataService(msalTester.client, accountInfo);
    mockGetOk.mockImplementationOnce(() => true);
    responseMock.json = jest.fn(() => [{
        id: "resource-group-id-1",
        name: "resource-group-name-1"
    },
    {
        id: "resource-group-id-2",
        name: "resource-group-name-2"
    }]) as jest.Mock;
    const resourceGroups = await dataService.getResourceGroups(url);

    expect(resourceGroups.length).toBe(2);
    expect(resourceGroups[0].id).toBe("resource-group-id-1");
    expect(resourceGroups[1].id).toBe("resource-group-id-2");
});