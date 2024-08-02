import { Configuration, RedirectRequest } from "@azure/msal-browser";

// Config object to be passed to Msal on creation
export const msalConfig: Configuration = {
    auth: {
        clientId: "6762e4b2-f06c-4ed6-9c4a-8742e8f177a9",
        authority: "https://login.microsoftonline.com/45f7fc17-a43c-4018-9724-11776458106a/oauth2/v2.0/authorize",
        redirectUri: "/dashboard",
        navigateToLoginRequestUrl: false,
        postLogoutRedirectUri: "/"
    },
    system: {
        allowNativeBroker: false // Disables WAM Broker
    }
};

// Add here scopes for id token to be used at MS Identity Platform endpoints.
export const loginRequest: RedirectRequest = {
    scopes: ["User.Read"]
};