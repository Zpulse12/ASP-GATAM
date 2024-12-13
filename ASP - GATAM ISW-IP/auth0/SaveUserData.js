/**
 * @param {Event} event - Details about the user and the context in which the login occurs.
 * @param {API} api - Interface to interact with Auth0 Actions.
 */
const axios = require('axios');

exports.onExecutePostUserRegistration = async (event, api) => {
  const apiEndpoint = event.secrets.API_ENDPOINT;

  const userData = {
    email: event.user.email,
    username: event.user.username || event.user.email,
    name: event.user.name,
    user_id: event.user.user_id,
    profile_picture: event.user.picture,
  };
  const headers = {
    "X-Api-Key": event.secrets.API_KEY
  }
  try {
    await axios.post(apiEndpoint, userData, {headers});
    console.log('User data sent successfully');
  } catch (error) {
    console.error('Failed to send user data. Rolling back registration');
    try {
      const AUTH0_CLIENT_ID = event.secrets.AUTH0_CLIENT_ID;
      const AUTH0_CLIENT_SECRET = event.secrets.AUTH0_CLIENT_SECRET;
      const AUTH0_DOMAIN = event.secrets.AUTH0_DOMAIN;
      const tokenResponse = await axios.post(`https://${AUTH0_DOMAIN}/oauth/token`, {
        client_id: AUTH0_CLIENT_ID,
        client_secret: AUTH0_CLIENT_SECRET,
        audience: `https://${AUTH0_DOMAIN}/api/v2/`,
        grant_type: 'client_credentials',
      });
      const token = tokenResponse.data.access_token;
      await axios.delete(`https://${AUTH0_DOMAIN}/api/v2/users/${event.user.user_id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      console.log('User successfully rolled back and removed from Auth0');
    } catch (rollbackError) {
      console.error('Rollback failed:', rollbackError.message);
    }
  }
};