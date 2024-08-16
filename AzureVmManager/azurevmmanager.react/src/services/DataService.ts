import { AccountInfo, IPublicClientApplication } from "@azure/msal-browser";
import Subscription from "../dataObjects/Subscription";
import { scopes } from "../authConfig";
import ResourceGroup from "../dataObjects/ResourceGroup";

export default class DataService {
    private instance: IPublicClientApplication;
    private accountInfo: AccountInfo;

    constructor(instance: IPublicClientApplication, accountInfo: AccountInfo) {
        this.instance = instance;
        this.accountInfo = accountInfo;
    }

    async get(url: string) {
        const accessTokenResponse = await this.instance.acquireTokenSilent({
            scopes: scopes,
            account: this.accountInfo,
        });
        const response = await fetch(`${window.location.origin}/api/${url}`, {
            headers: [["Authorization", `Bearer ${accessTokenResponse.accessToken}`]]
        });
        if (response.ok)
            return response;
        throw new Error(await response.text());
    }

    async getSubscriptions(url: string): Promise<Subscription[]> {
        const response = await this.get(url);
        return response.json();
    }

    async getResourceGroups(url: string): Promise<ResourceGroup[]> {
        const response = await this.get(url);
        return response.json();
    }
}