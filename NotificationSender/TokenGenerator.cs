﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Jose;

namespace NotificationSender
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly Dictionary<string, object> _header;
        private readonly Dictionary<string, object> _payload;

        private DateTimeOffset _creationTime = DateTimeOffset.MinValue;
        private readonly int _tokenLifeTimeInMinutes = 1;

        private string _validToken;
        private readonly CngKey _cngPrivateKey;

        public TokenGenerator(string keyId, string privateSigningKey, string teamId)
        {
            _header = new Dictionary<string, object>
            {
                {"kid", keyId},
                {"alg", "ES256"},
                {"typ", "JWT"},
            };

            _payload = new Dictionary<string, object>
            {
                {"iss", teamId},
            };

            _cngPrivateKey =
                CngKey.Import(Convert.FromBase64String(privateSigningKey), CngKeyBlobFormat.Pkcs8PrivateBlob);
        }

        public string GetValidToken()
        {
            var elapsedTimeInMinutes = (DateTimeOffset.UtcNow - _creationTime).Minutes;

            if (elapsedTimeInMinutes >= _tokenLifeTimeInMinutes)
                _validToken = CreateJwtToken();

            return _validToken;
        }

        private string CreateJwtToken()
        {
            _creationTime = DateTimeOffset.UtcNow;
            var issuedAt = _creationTime.ToUnixTimeSeconds();
            var expirationTime = _creationTime.AddMinutes(_tokenLifeTimeInMinutes).ToUnixTimeSeconds();

            _payload["iat"] = issuedAt;
            _payload["exp"] = expirationTime;

            return JWT.Encode(_payload, _cngPrivateKey, JwsAlgorithm.ES256, _header);
        }
    }
}