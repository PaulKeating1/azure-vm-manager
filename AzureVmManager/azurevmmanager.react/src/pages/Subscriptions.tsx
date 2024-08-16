import { InteractionType } from "@azure/msal-browser";
import { MsalAuthenticationTemplate, useMsal } from "@azure/msal-react";
import DataService from "../services/DataService";
import { useEffect, useState } from "react";
import Subscription from "../dataObjects/Subscription";
import Loading from "../components/Loading";
import TableContainer from "@mui/material/TableContainer";
import Table from "@mui/material/Table";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableCell from "@mui/material/TableCell";
import TableBody from "@mui/material/TableBody";
import Paper from "@mui/material/Paper"
import { Link } from "react-router-dom";
import Alert from "@mui/material/Alert";

export default function Subscriptions() {
    const { instance, accounts } = useMsal();
    const [subscriptions, setSubscriptions] = useState<Subscription[]>();
    const [erroMessage, setErrorMessage] = useState<string>();

    useEffect(() => {
        const dataService = new DataService(instance, accounts[0]);
        const getSubscriptions = async () => {
            try {
                const subscriptions = await dataService.getSubscriptions("Subscriptions");
                setSubscriptions(subscriptions);
            } catch (error) {
                const message = error instanceof Error ? error.message : String(error);
                setErrorMessage(`Error loading subscriptions: ${message}`);
            }
        };

        getSubscriptions();

        return () => {
            // this now gets called when the component unmounts
        };
    }, [instance, accounts]);

    if (erroMessage)
        return <Alert severity="error">{erroMessage}</Alert>

    if (!subscriptions)
        return <Loading />;

    return (
        <MsalAuthenticationTemplate
            interactionType={InteractionType.Redirect}
        >
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Subscription name</TableCell>
                            <TableCell>Subscription ID</TableCell>                        
                        </TableRow>
                    </TableHead>
                    <TableBody>
                    {subscriptions.map((subscription) => (
                        <TableRow key={subscription.id}>
                            <TableCell><Link to={`/resource-groups/${subscription.id}`}>{subscription.name}</Link></TableCell>
                            <TableCell>{subscription.id}</TableCell>                        
                        </TableRow>
                    ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </MsalAuthenticationTemplate>
    );
}