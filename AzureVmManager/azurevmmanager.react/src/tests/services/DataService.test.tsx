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

beforeEach(() => {
    global.fetch = jest.fn((requestUrl, options) =>{
        Object.defineProperty(responseMock, 'url', {
            get: () => requestUrl
        });
        Object.defineProperty(responseMock, 'headers', {
            get: () => options.headers
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
    const fetchResponse = await dataService.get(url);
    const headers = (fetchResponse.headers as unknown) as [[]];
    const firstHeader = headers[0] as Array<string>;
    const firstHeaderKey = firstHeader.length > 0 ? firstHeader[0] : null;
    const firstHeaderValue = firstHeader.length > 1 ? firstHeader[1] : null;

    expect(fetchResponse.url).toBe(`api/${url}`);
    expect(firstHeaderKey).toBe("Authorization");
    expect(firstHeaderValue).toBe(`Bearer ${accessToken}`);
});

test("GetSubscriptions method returns subscriptions", async () => {
    const dataService = new DataService(msalTester.client, accountInfo);
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