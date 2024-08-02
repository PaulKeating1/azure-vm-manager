import { Configuration, RedirectRequest } from "@azure/msal-browser";

// Config object to be passed to Msal on creation
export const msalConfig: Configuration = {
    auth: {
        clientId: "[add-client-id-here]",
        authority: "https://login.microsoftonline.com/[add-tenant-id-here]/oauth2/v2.0/authorize",
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