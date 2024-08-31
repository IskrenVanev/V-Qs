import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import VoteList from './VoteList.jsx';
import About from './About.jsx';

function App() {
    return (
        <Router>
            <div className="App">
                <header className="app-header">
                    <h1 className="heading">V&Qs</h1>
                    <nav className="nav">
                        <ul className="nav-list">
                            <li className="nav-item">
                                <Link to="/">Home</Link>
                            </li>
                            <li className="nav-item">
                                <Link to="/about">About</Link>
                            </li>
                        </ul>
                    </nav>
                </header>
                <Routes>
                    <Route path="/" element={<VoteList />} />
                    <Route path="/about" element={<About />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;