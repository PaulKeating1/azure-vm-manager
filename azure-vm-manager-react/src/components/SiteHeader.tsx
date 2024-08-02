import { NavLink } from "react-router-dom";
import "./SiteHeader.css"
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";

export default function SiteHeader() {

  const { instance } = useMsal();

  function signIn() {      
    instance.loginRedirect();
  }

  function signOut() {      
    instance.logoutRedirect();
  }

    return (
      <div>
        <header className="site-header">
          Azure VM Manager
        </header>
        <nav className="site-header-menu">
          <div>
            <NavLink to="/" className="site-header-menu-item">Home</NavLink>
          </div>
          <div>            
            <UnauthenticatedTemplate>
              <button className="site-header-sign-in-out" onClick={signIn}>Sign In</button>
            </UnauthenticatedTemplate>
            <AuthenticatedTemplate>
              <button className="site-header-sign-in-out" onClick={signOut}>Sign Out</button>
            </AuthenticatedTemplate>
          </div>          
        </nav>
      </div>
    );
  }
  