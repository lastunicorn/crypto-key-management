using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.PresentMain.PresentMainUseCaseTests;

public class QueryTests
{
    private readonly Mock<ISignatureKeyRepository> signatureKeyRepository;
    private readonly Mock<IApplicationState> applicationState;
    private readonly PresentMainUseCase useCase;

    public QueryTests()
    {
        signatureKeyRepository = new Mock<ISignatureKeyRepository>();
        applicationState = new Mock<IApplicationState>();

        useCase = new PresentMainUseCase(
            signatureKeyRepository.Object,
            applicationState.Object);
    }

    [Fact]
    public async Task Query_ShouldRetrieveSignatureKeysFromRepository()
    {
        // Arrange
        PresentMainRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(2);

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        await useCase.Query(request);

        // Assert
        signatureKeyRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldReturnCorrectNumberOfSignatureKeys()
    {
        // Arrange
        PresentMainRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(3);

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.NotNull(response.SignatureKeys);
        Assert.Equal(3, response.SignatureKeys.Count);
    }

    [Fact]
    public async Task Query_WithEmptyRepository_ShouldReturnEmptyList()
    {
        // Arrange
        PresentMainRequest request = new();

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.NotNull(response.SignatureKeys);
        Assert.Empty(response.SignatureKeys);
    }

    [Fact]
    public async Task Query_ShouldConvertKeyPairsToDto()
    {
        // Arrange
        PresentMainRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(1);
        KeyPair expectedKey = mockKeys.First();

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.Single(response.SignatureKeys);
        KeyPairDto resultDto = response.SignatureKeys.First();

        Assert.Equal(expectedKey.Id, resultDto.Id);
        Assert.Equal(expectedKey.CreatedDate, resultDto.CreatedDate);
        Assert.Equal(expectedKey.PrivateKey, resultDto.PrivateKey);
        Assert.Equal(expectedKey.PublicKey, resultDto.PublicKey);
    }

    [Fact]
    public async Task Query_WithCurrentSignatureKey_ShouldReturnCorrectSelectedId()
    {
        // Arrange
        PresentMainRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(2);
        KeyPair currentKey = mockKeys.First();

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(currentKey);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.Equal(currentKey.Id, response.SelectedSignatureKeyId);
    }

    [Fact]
    public async Task Query_WithNullCurrentSignatureKey_ShouldReturnNullSelectedId()
    {
        // Arrange
        PresentMainRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(2);

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.Null(response.SelectedSignatureKeyId);
    }

    [Fact]
    public async Task Query_ShouldAccessCurrentSignatureKeyFromApplicationState()
    {
        // Arrange
        PresentMainRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(1);

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        await useCase.Query(request);

        // Assert
        applicationState.Verify(x => x.CurrentSignatureKey, Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Query_WithVariousNumberOfKeys_ShouldReturnCorrectCount(int keyCount)
    {
        // Arrange
        PresentMainRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(keyCount);

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.Equal(keyCount, response.SignatureKeys.Count);
    }

    [Fact]
    public async Task Query_WithDuplicateKeyIds_ShouldPreserveAllKeys()
    {
        // Arrange
        PresentMainRequest request = new();
        Guid duplicateId = Guid.NewGuid();
        IEnumerable<KeyPair> mockKeys = new List<KeyPair>
        {
            CreateKeyPair(duplicateId, DateTime.Now),
            CreateKeyPair(duplicateId, DateTime.Now.AddDays(1))
        };

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.Equal(2, response.SignatureKeys.Count);
        Assert.All(response.SignatureKeys, dto => Assert.Equal(duplicateId, dto.Id));
    }

    [Fact]
    public async Task Query_WithKeysHavingNullProperties_ShouldHandleGracefully()
    {
        // Arrange
        PresentMainRequest request = new();
        IEnumerable<KeyPair> mockKeys = new List<KeyPair>
        {
            new KeyPair
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                PrivateKey = null,
                PublicKey = null
            }
        };

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);
        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.Single(response.SignatureKeys);
        KeyPairDto resultDto = response.SignatureKeys.First();
        Assert.Null(resultDto.PrivateKey);
        Assert.Null(resultDto.PublicKey);
    }

    [Fact]
    public async Task Query_ShouldReturnTaskWithValidResponse()
    {
        // Arrange
        PresentMainRequest request = new();

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        Task<PresentMainResponse> responseTask = useCase.Query(request);

        // Assert
        Assert.NotNull(responseTask);
        Assert.True(responseTask.IsCompletedSuccessfully);

        PresentMainResponse response = await responseTask;
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Query_WithRepositoryException_ShouldPropagateException()
    {
        // Arrange
        PresentMainRequest request = new();
        Exception expectedException = new InvalidOperationException("Repository error");

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Throws(expectedException);

        // Act & Assert
        Exception actualException = await Assert.ThrowsAsync<InvalidOperationException>(
          () => useCase.Query(request));

        Assert.Same(expectedException, actualException);
    }

    [Fact]
    public async Task Query_WithApplicationStateException_ShouldPropagateException()
    {
        // Arrange
        PresentMainRequest request = new();

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        Exception expectedException = new InvalidOperationException("Application state error");
        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Throws(expectedException);

        // Act & Assert
        Exception actualException = await Assert.ThrowsAsync<InvalidOperationException>(
          () => useCase.Query(request));

        Assert.Same(expectedException, actualException);
    }

    [Fact]
    public async Task Query_WithRealScenario_ShouldWorkCorrectly()
    {
        // Arrange
        PresentMainRequest request = new();
        KeyPair currentKey = CreateKeyPair(Guid.NewGuid(), DateTime.Now);
        IEnumerable<KeyPair> allKeys = new List<KeyPair>
        { 
            currentKey,
            CreateKeyPair(Guid.NewGuid(), DateTime.Now.AddDays(1))
        };

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(allKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(currentKey);

        // Act
        PresentMainResponse response = await useCase.Query(request);

        // Assert
        Assert.Equal(2, response.SignatureKeys.Count);
        Assert.Equal(currentKey.Id, response.SelectedSignatureKeyId);

        // Verify that the DTOs contain the correct data
        KeyPairDto currentKeyDto = response.SignatureKeys.First(dto => dto.Id == currentKey.Id);
        Assert.Equal(currentKey.CreatedDate, currentKeyDto.CreatedDate);
        Assert.Equal(currentKey.PrivateKey, currentKeyDto.PrivateKey);
        Assert.Equal(currentKey.PublicKey, currentKeyDto.PublicKey);
    }

    private static IEnumerable<KeyPair> CreateMockKeyPairs(int count)
    {
        List<KeyPair> keys = [];
        
        for (int i = 0; i < count; i++)
            keys.Add(CreateKeyPair(Guid.NewGuid(), DateTime.Now.AddDays(i)));
        
        return keys;
    }

    private static KeyPair CreateKeyPair(Guid id, DateTime createdDate)
    {
        return new KeyPair
        {
            Id = id,
            CreatedDate = createdDate,
            PrivateKey = GenerateRandomBytes(32),
            PublicKey = GenerateRandomBytes(32),
            PrivateKeyPath = $"private_{id}.key",
            PublicKeyPath = $"public_{id}.key"
        };
    }

    private static byte[] GenerateRandomBytes(int length)
    {
        Random random = new();
        byte[] bytes = new byte[length];
        random.NextBytes(bytes);
        return bytes;
    }
}