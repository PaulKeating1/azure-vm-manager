import { Outlet, RouteObject } from "react-router-dom";
import SiteHeader from './components/SiteHeader';
import { AuthenticatedTemplate } from "@azure/msal-react";
import Subscriptions from './pages/Subscriptions';
import AccountNav from './components/AccountNav';
import Home from './pages/Home';
import Dashboard from './pages/Dashboard';

const routesConfig: RouteObject[] = [{
    path: "/",
    element: (<>
    <SiteHeader />
    <div className="main-layout">
        <AuthenticatedTemplate>
            <AccountNav />
        </AuthenticatedTemplate>        
        <main className="main-content">
            <Outlet />
        </main>
    </div>    
    </>),
    children: [
        { path: "/", element: <Home /> },
        { path: "/dashboard", element: <Dashboard /> },
        { path: "/subscriptions", element: <Subscriptions /> },
        { path: "/resource-groups/:subscriptionId", element: <>Resource Groups</>}
      ],
}];

export default routesConfig;