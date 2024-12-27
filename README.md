# KeyPairGenerator

## Complete Cryptography Workflow

### Generate RSA Key Pair
The `KeyPairGeneratorService.GenerateKeyPair()` method generates a public key and a private key using the RSA algorithm.

- **Public Key:** Used to encrypt data.
- **Private Key:** Used to decrypt data.

---

### Store and Retrieve Key Pair
- **Store:**  
  The private key is encrypted using AES and stored in the `privateKey.bson` file via `SecureStorage.StorePrivateKey()`.

- **Retrieve:**  
  The private key can be retrieved and decrypted using `SecureStorage.RetrievePrivateKey()`.

---

### Generate Secret
The secret (a symmetric key) is generated using the `SecretGenerator.GenerateSecret()` method.  
This secret is used to encrypt objects and files.

---

### Store and Retrieve Secret
- **Store:**  
  The secret is encrypted with AES and stored in the `secret.bson` file via `SecureStorage.StoreSecret()`.

- **Retrieve:**  
  The secret can be retrieved and decrypted using `SecureStorage.RetrieveSecret()`.

---

### Encrypt and Decrypt Secret
- **Encrypt:**  
  The secret can be encrypted with the RSA public key using `SecretGenerator.EncryptSecret()`.

- **Decrypt:**  
  It can be decrypted with the RSA private key using `SecretGenerator.DecryptSecret()`.

---

### Encrypt and Decrypt Objects
- **Encrypt:**  
  Binary data (objects) are encrypted using the decrypted secret and AES via `SecretGenerator.EncryptObject()`.

- **Decrypt:**  
  They can be decrypted using `SecretGenerator.DecryptObject()`.

---

### Encrypt and Decrypt Files
- **Encrypt:**  
  Files are encrypted similarly to objects, using `SecretGenerator.EncryptFile()`.

- **Decrypt:**  
  Files are decrypted similarly to objects, using `SecretGenerator.DecryptFile()`.

---