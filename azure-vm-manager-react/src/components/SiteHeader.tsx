import { NavLink } from "react-router-dom";
import "./SiteHeader.css"

export default function SiteHeader() {
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
            <NavLink to="" className="site-header-menu-item">Sign In</NavLink>
          </div>          
        </nav>
      </div>
    );
  }
  