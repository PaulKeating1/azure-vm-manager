import { InteractionType } from "@azure/msal-browser";
import { MsalAuthenticationTemplate, useMsal } from "@azure/msal-react";
import DataService from "../services/DataService";
import { useEffect, useState } from "react";
import Subscription from "../dataObjects/Subscription";

function Subscriptions() {
    const { instance, accounts } = useMsal();
    const [subscriptions, setSubscriptions] = useState<Subscription[]>();

    useEffect(() => {
        const dataService = new DataService(instance, accounts[0]);
        const getSubscriptions = async () => {
            const subscriptions = await dataService.getSubscriptions("Subscriptions");
            setSubscriptions(subscriptions);
        };
        getSubscriptions();

        return () => {
            // this now gets called when the component unmounts
        };
    }, [instance, accounts]);

    if (!subscriptions)
        return <div>Loading...</div>;

    return (
        <MsalAuthenticationTemplate
            interactionType={InteractionType.Redirect}
        >
            {subscriptions.map((subscription) => (
                <p key={subscription.id}>{subscription.name} - {subscription.id}</p>
            ))}
        </MsalAuthenticationTemplate>
    );
}

export default Subscriptions;