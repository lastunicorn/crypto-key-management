using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.PresentSigningPage.PresentSigningPageUseCaseTests;

public class QueryTests
{
    private readonly Mock<ICryptoKeyRepository> cryptoKeyRepository;
    private readonly Mock<IApplicationState> applicationState;
    private readonly PresentSigningPageUseCase useCase;

    public QueryTests()
    {
        cryptoKeyRepository = new Mock<ICryptoKeyRepository>();
        applicationState = new Mock<IApplicationState>();

        useCase = new PresentSigningPageUseCase(
            cryptoKeyRepository.Object,
            applicationState.Object);
    }

    [Fact]
    public async Task Query_ShouldRetrieveSignatureKeysFromRepository()
    {
        // Arrange
        PresentSigningPageRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(2);

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        await useCase.Query(request);

        // Assert
        cryptoKeyRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldReturnCorrectNumberOfSignatureKeys()
    {
        // Arrange
        PresentSigningPageRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(3);

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.NotNull(response.KeyPairs);
        Assert.Equal(3, response.KeyPairs.Count);
    }

    [Fact]
    public async Task Query_WithEmptyRepository_ShouldReturnEmptyList()
    {
        // Arrange
        PresentSigningPageRequest request = new();

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.NotNull(response.KeyPairs);
        Assert.Empty(response.KeyPairs);
    }

    [Fact]
    public async Task Query_ShouldConvertKeyPairsToDto()
    {
        // Arrange
        PresentSigningPageRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(1);
        KeyPair expectedKey = mockKeys.First();

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.Single(response.KeyPairs);
        KeyPairDto resultDto = response.KeyPairs.First();

        Assert.Equal(expectedKey.Id, resultDto.Id);
        Assert.Equal(expectedKey.CreatedDate, resultDto.CreatedDate);
        Assert.Equal(expectedKey.PrivateKey, resultDto.PrivateKey);
        Assert.Equal(expectedKey.PublicKey, resultDto.PublicKey);
    }

    [Fact]
    public async Task Query_WithCurrentSignatureKey_ShouldReturnCorrectSelectedId()
    {
        // Arrange
        PresentSigningPageRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(2);
        KeyPair currentKey = mockKeys.First();

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(currentKey);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.Equal(currentKey.Id, response.SelectedKeyPairId);
    }

    [Fact]
    public async Task Query_WithNullCurrentSignatureKey_ShouldReturnNullSelectedId()
    {
        // Arrange
        PresentSigningPageRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(2);

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.Null(response.SelectedKeyPairId);
    }

    [Fact]
    public async Task Query_ShouldAccessCurrentSignatureKeyFromApplicationState()
    {
        // Arrange
        PresentSigningPageRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(1);

        cryptoKeyRepository
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
        PresentSigningPageRequest request = new();
        IEnumerable<KeyPair> mockKeys = CreateMockKeyPairs(keyCount);

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.Equal(keyCount, response.KeyPairs.Count);
    }

    [Fact]
    public async Task Query_WithDuplicateKeyIds_ShouldPreserveAllKeys()
    {
        // Arrange
        PresentSigningPageRequest request = new();
        Guid duplicateId = Guid.NewGuid();
        IEnumerable<KeyPair> mockKeys = new List<KeyPair>
        {
            CreateKeyPair(duplicateId, DateTime.Now),
            CreateKeyPair(duplicateId, DateTime.Now.AddDays(1))
        };

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.Equal(2, response.KeyPairs.Count);
        Assert.All(response.KeyPairs, dto => Assert.Equal(duplicateId, dto.Id));
    }

    [Fact]
    public async Task Query_WithKeysHavingNullProperties_ShouldHandleGracefully()
    {
        // Arrange
        PresentSigningPageRequest request = new();
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

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(mockKeys);
        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.Single(response.KeyPairs);
        KeyPairDto resultDto = response.KeyPairs.First();
        Assert.Null(resultDto.PrivateKey);
        Assert.Null(resultDto.PublicKey);
    }

    [Fact]
    public async Task Query_ShouldReturnTaskWithValidResponse()
    {
        // Arrange
        PresentSigningPageRequest request = new();

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act
        Task<PresentSigningPageResponse> responseTask = useCase.Query(request);

        // Assert
        Assert.NotNull(responseTask);
        Assert.True(responseTask.IsCompletedSuccessfully);

        PresentSigningPageResponse response = await responseTask;
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Query_WithRepositoryException_ShouldPropagateException()
    {
        // Arrange
        PresentSigningPageRequest request = new();
        Exception expectedException = new InvalidOperationException("Repository error");

        cryptoKeyRepository
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
        PresentSigningPageRequest request = new();

        cryptoKeyRepository
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
        PresentSigningPageRequest request = new();
        KeyPair currentKey = CreateKeyPair(Guid.NewGuid(), DateTime.Now);
        IEnumerable<KeyPair> allKeys = new List<KeyPair>
        { 
            currentKey,
            CreateKeyPair(Guid.NewGuid(), DateTime.Now.AddDays(1))
        };

        cryptoKeyRepository
            .Setup(x => x.GetAll())
            .Returns(allKeys);

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(currentKey);

        // Act
        PresentSigningPageResponse response = await useCase.Query(request);

        // Assert
        Assert.Equal(2, response.KeyPairs.Count);
        Assert.Equal(currentKey.Id, response.SelectedKeyPairId);

        // Verify that the DTOs contain the correct data
        KeyPairDto currentKeyDto = response.KeyPairs.First(dto => dto.Id == currentKey.Id);
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