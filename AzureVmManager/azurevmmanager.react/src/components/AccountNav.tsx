import { NavLink } from "react-router-dom";
import "./AccountNav.css"

export default function AccountNav() {
    return (
        <nav>
            <div className="account-menu">
                <NavLink to="/subscriptions" className="account-menu-item">Subscriptions</NavLink>
            </div>
        </nav>
    );
}