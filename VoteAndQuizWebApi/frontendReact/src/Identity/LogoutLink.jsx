import { useNavigate } from "react-router-dom";
import { useAuth } from "../AuthContext.jsx"; // Assuming useAuth provides the necessary functions

function LogoutLink({ children }) {
    const navigate = useNavigate();
    const { logout } = useAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();
    
        try {
            const response = await fetch("https://localhost:7055/logout", {
                method: "POST",
                credentials: "include", // Ensure cookies are included
                headers: {
                    "Content-Type": "application/json", // Adjust headers if necessary
                },
            });
    
            if (response.ok) {
                logout(); // Update the authentication state
                navigate('/'); // Redirect to home after successful logout
            } else {
                console.error("Logout failed.");
            }
        } catch (error) {
            console.error("An error occurred during logout:", error);
        }
    };
    
    return (
        <button className="nav-item" onClick={handleSubmit}>
            {children}
        </button>
    );
}

export default LogoutLink;