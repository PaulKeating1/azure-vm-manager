import {
    RouterProvider,
    createBrowserRouter
} from "react-router-dom";
import './App.css';
import { MsalProvider } from "@azure/msal-react";
import { IPublicClientApplication } from "@azure/msal-browser";
import routesConfig from './routesConfig';

export type AppProps = {
    pca: IPublicClientApplication;
};

const router = createBrowserRouter(routesConfig);

function App({ pca }: AppProps) {

    return (
        <MsalProvider instance={pca}>
            <div className="App">
                <RouterProvider router={router}/>
            </div>   
        </MsalProvider>
    );
}

export default App;
