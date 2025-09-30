import { Navigate } from 'react-router-dom';
import { useAuth } from './AuthContext.jsx';

function ProtectedRoute({children}){
    const { isAuthenticated } = useAuth(); // Destructure the isAuthenticated state
    console.log(isAuthenticated);
    if (!isAuthenticated){
        return <Navigate to="/Login" replace/>
    }
    return children;
}

export default ProtectedRoute;