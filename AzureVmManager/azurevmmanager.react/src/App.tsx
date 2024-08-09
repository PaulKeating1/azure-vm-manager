import Home from './pages/Home';
import Dashboard from './pages/Dashboard';
import {
    BrowserRouter as Router,
    Routes,
    Route
} from "react-router-dom";
import './App.css';
import SiteHeader from './components/SiteHeader';
import { AuthenticatedTemplate, MsalProvider } from "@azure/msal-react";
import { IPublicClientApplication } from "@azure/msal-browser";
import Subscriptions from './pages/Subscriptions';
import AccountNav from './components/AccountNav';

export type AppProps = {
    pca: IPublicClientApplication;
};

function App({ pca }: AppProps) {

    return (
        <MsalProvider instance={pca}>
            <div className="App">
                <Router>
                    <SiteHeader />
                    <div className="main-layout">
                        <AuthenticatedTemplate>
                            <AccountNav />
                        </AuthenticatedTemplate>        
                        <main className="main-content">
                            <Routes>
                                <Route path="/" element={<Home />} />
                                <Route path="/dashboard" element={<Dashboard />} />
                                <Route path="/subscriptions" element={<Subscriptions />} />
                            </Routes>
                        </main>
                    </div>
                </Router>
            </div>   
        </MsalProvider>
    );
}

export default App;
