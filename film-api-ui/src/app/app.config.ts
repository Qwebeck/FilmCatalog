export default {
  oidc: {
    clientId: '0oag0zkk4PsS8o62g4x6',
    issuer: 'https://dev-221155.okta.com/oauth2/default',
    redirectUri: 'http://localhost:4200/implicit/callback',
    scopes: ['openid', 'profile', 'email'],
    testing: {
      disableHttpsCheck: false
    }
  },
  resourceServer: {
    messagesUrl: 'http://localhost:4200/api/messages',
  },
};