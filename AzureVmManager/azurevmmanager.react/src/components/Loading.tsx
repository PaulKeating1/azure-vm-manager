import "./Loading.css"
import CircularProgress from "@mui/material/CircularProgress";

export default function Loading() {
    return (
        <div className="loading-container">
            <CircularProgress /> Loading...
        </div>
    );
}