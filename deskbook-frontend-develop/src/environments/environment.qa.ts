// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  // 1Authority Configuration
  client_id: 'DeskBookQA',
  // openid profile - UserProfile
  scope:
    'openid profile UserProfile 1AuthorityApi 1RPPPolicyServerApi DeskBookApi',
  response_type: 'id_token token',
  authority: 'https://1authority-interns.1rivet.com:5004/',
  redirect_uri: 'https://deskbook-qa.1rivet.com:9447/',
  secret: 'secret1',
  grant_type: 'implicit',
  // Policy server config
  policy_url: 'http://103.249.120.58:8503/api/userPolicy/',
  policy_name: 'PMCPolicy',
  apiUrl: 'http://103.249.120.58:8508/api/',
  authority_register_uri:
    'https://deskbook-admin-service-qa.1rivet.com:9448/api/',
  acr_values: 'tenant:92E838C3-EC40-486F-B126-BBEC78E7D99C',
  userprofile_uri: 'https://qa-deskbook-userapi.1rivet.com:9446/api/deskbook/',
};
