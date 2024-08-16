import { InteractionType } from "@azure/msal-browser";
import { MsalAuthenticationTemplate, useMsal } from "@azure/msal-react";
import DataService from "../services/DataService";
import { useEffect, useState } from "react";
import ResourceGroup from "../dataObjects/ResourceGroup";
import Loading from "../components/Loading";
import TableContainer from "@mui/material/TableContainer";
import Table from "@mui/material/Table";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableCell from "@mui/material/TableCell";
import TableBody from "@mui/material/TableBody";
import Paper from "@mui/material/Paper"
import { useParams } from "react-router-dom";
import Alert from "@mui/material/Alert";

export default function ResourceGroups() {
    const { instance, accounts } = useMsal();
    const [resourceGoups, setResourceGoups] = useState<ResourceGroup[]>();
    const { subscriptionId } = useParams();
    const [erroMessage, setErrorMessage] = useState<string>();

    useEffect(() => {
        const dataService = new DataService(instance, accounts[0]);
        const getResourceGroups = async () => {
            try {
                const resourceGoups = await dataService.getResourceGroups(`ResourceGroups/${subscriptionId}`);
                setResourceGoups(resourceGoups);
            } catch (error) {
                const message = error instanceof Error ? error.message : String(error);
                setErrorMessage(`Error loading resource groups: ${message}`);
            }
        };
        getResourceGroups();

        return () => {
            // this now gets called when the component unmounts
        };
    }, [instance, accounts, subscriptionId]);

    if (erroMessage)
        return <Alert severity="error">{erroMessage}</Alert>
        
    if (!resourceGoups)
        return <Loading />;

    return (
        <MsalAuthenticationTemplate
            interactionType={InteractionType.Redirect}
        >
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell>Subscription</TableCell> 
                            <TableCell>Location</TableCell>                        
                        </TableRow>
                    </TableHead>
                    <TableBody>
                    {resourceGoups.map((resourceGoup) => (
                        <TableRow key={resourceGoup.id}>
                            <TableCell>{resourceGoup.name}</TableCell>
                            <TableCell>{resourceGoup.subscriptionName}</TableCell>
                            <TableCell>{resourceGoup.location}</TableCell>
                        </TableRow>
                    ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </MsalAuthenticationTemplate>
    );
}