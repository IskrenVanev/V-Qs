import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from '../AuthContext.jsx'; // Import the useAuth hook

function Login() {
    // State variables for email and password
    const { login } = useAuth(); // Get the login function from context
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [rememberme, setRememberme] = useState(false);
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value, checked } = e.target;
        if (name === "email") setEmail(value);
        if (name === "password") setPassword(value);
        if (name === "rememberme") setRememberme(checked);
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        if (!email || !password) {
            setError("Please fill in all fields.");
            return;
        }

        setError(""); // Clear error message

        // Set the login URL based on rememberme
        const loginUrl = `https://localhost:7055/login?${rememberme ? "useCookies=true" : "useSessionCookies=true"}`;
        
        fetch(loginUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                email,
                password,
            }),
            credentials: "include", // Include cookies in the request
        })
            .then((response) => {
                if (response.ok) {
                    // Successfully logged in, redirect to home
                    login();
                } else {
                    setError("Error Logging In.");
                }
            })
            .catch((error) => {
                console.error("Login error:", error);
                setError("Error Logging in: " + error.message);
            });
    };

    return (
        <div className="containerbox">
            <h3>Login</h3>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="email">Email:</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={email}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        value={password}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <input
                        type="checkbox"
                        id="rememberme"
                        name="rememberme"
                        checked={rememberme}
                        onChange={handleChange}
                    />
                    <label htmlFor="rememberme">Remember Me</label>
                </div>
                <div>
                    <button type="submit">Login</button>
                </div>
                {error && <p className="error">{error}</p>}
            </form>
        </div>
    );
}

export default Login;