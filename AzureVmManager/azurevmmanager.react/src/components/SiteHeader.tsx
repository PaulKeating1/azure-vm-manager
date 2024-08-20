import { NavLink } from "react-router-dom";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import Link from "@mui/material/Link";

export default function SiteHeader() {

    const { instance } = useMsal();

    function signIn() {        
        instance.loginRedirect();
    }

    function signOut() {
        instance.logoutRedirect();
    }

    return (
        <AppBar position="static">
            <Box alignItems="center" alignSelf="center">
                <Typography variant="h2" component="div" sx={{ flexGrow: 1 }}>
                    Azure VM Manager
                </Typography>
            </Box>
            <Toolbar>
                <Box sx={{ flexGrow: 1 }}>
                    <Link color="inherit" underline="none" component={NavLink} to="/">
                        <Typography>Home</Typography>
                    </Link>
                </Box>
                <Box sx={{ flexGrow: 0 }}>
                    <UnauthenticatedTemplate>
                        <Button color="inherit" onClick={signIn}>
                            <Typography>Login</Typography>
                        </Button>
                    </UnauthenticatedTemplate>
                    <AuthenticatedTemplate>
                        <Button color="inherit" onClick={signOut}>
                            <Typography>Logout</Typography>
                        </Button>
                    </AuthenticatedTemplate>
                </Box>
            </Toolbar>
        </AppBar>
    );
}
