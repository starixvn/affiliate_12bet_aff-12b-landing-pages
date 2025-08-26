import React from 'react';
import ContactForm from './ContactForm';

const App = () => {
  return (
    <div className="react-app">
      <h1>Welcome to Fanex Affiliate Landing Pages</h1>
      <p>This is now powered by React!</p>
      
      <div className="row mt-4">
        <div className="col-md-6">
          <ContactForm />
        </div>
        <div className="col-md-6">
          <div className="card">
            <div className="card-body">
              <h5 className="card-title">Migration Complete</h5>
              <p className="card-text">
                jQuery has been successfully removed and replaced with React. 
                This form demonstrates client-side validation without jQuery.
              </p>
              <ul className="list-unstyled">
                <li>✅ jQuery removed</li>
                <li>✅ React 18.2.0 added</li>
                <li>✅ Webpack bundling configured</li>
                <li>✅ Form validation in React</li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default App;
