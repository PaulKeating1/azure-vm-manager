import { AccountInfo, IPublicClientApplication } from "@azure/msal-browser";
import Subscription from "../dataObjects/Subscription";
import { scopes } from "../authConfig";

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
        return fetch(`api/${url}`, {
            headers: [["Authorization", `Bearer ${accessTokenResponse.accessToken}`]]
        });
    }

    async getSubscriptions(url: string): Promise<Subscription[]> {
        const response = this.get(url);
        return (await response).json();
    }
}