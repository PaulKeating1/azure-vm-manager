import Home from './pages/Home';
import Dashboard from './pages/Dashboard';
import {
    BrowserRouter as Router,
    Routes,
    Route
} from "react-router-dom";
import './App.css';
import SiteHeader from './components/SiteHeader';
import { MsalProvider } from "@azure/msal-react";
import { IPublicClientApplication } from "@azure/msal-browser";

export type AppProps = {
    pca: IPublicClientApplication;
};

function App({ pca }: AppProps) {

    return (
        <MsalProvider instance={pca}>
            <div className="App">
                <Router>
                    <SiteHeader />
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/dashboard" element={<Dashboard />} />
                    </Routes>
                </Router>
            </div>
        </MsalProvider>
    );
}

export default App;
