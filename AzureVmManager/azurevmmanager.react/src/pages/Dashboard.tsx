import { MsalAuthenticationTemplate } from "@azure/msal-react";
import { InteractionType } from "@azure/msal-browser";

export default function Dashboard() {
    return (
        <MsalAuthenticationTemplate
            interactionType={InteractionType.Redirect}
        >
            <p>Dashboard</p>
        </MsalAuthenticationTemplate>
    );
}