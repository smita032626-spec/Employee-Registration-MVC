ALTER PROCEDURE [dbo].[SPEmployeeManagement]
(
    @Flag NVARCHAR(50) = NULL,
    @Id INT = NULL,
    @Name NVARCHAR(100) = NULL,
    @Email NVARCHAR(100) = NULL,
    @PhoneNumber NVARCHAR(15) = NULL,
    @DOB DATE = NULL,
    @Address NVARCHAR(200) = NULL,
    @TalukaId INT = NULL,
    @DepartmentId INT = NULL,
    @DesignationId INT = NULL,
    @Pincode NVARCHAR(10) = NULL,
    @CityId INT = NULL,
    @FilePath NVARCHAR(200) = NULL,
	@DistrictId INT = NULL,
    @IsActive BIT = NULL
)
AS
BEGIN

    -- FETCH ALL EMPLOYEES
    IF(@Flag = 'FetchEmployeeDetails')
    BEGIN
       SELECT 
    E.Id,
    E.Name,
    E.Email,
    E.PhoneNumber,
    E.DOB,
    E.Address,

    E.CityId,
    C.CityName,

    E.DistrictId,
    D.DistrictName,

    E.TalukaId,
    T.TalukaName,

    E.DepartmentId,
    DEP.DepartmentName,

    E.DesignationId,
    DES.DesignationName,

    E.Pincode,
    E.FilePath,
    E.IsActive

FROM TblEmployees E
LEFT JOIN TblCities C ON E.CityId = C.CityId
LEFT JOIN TblDistricts D ON E.DistrictId = D.DistrictId
LEFT JOIN TblTalukas T ON E.TalukaId = T.TalukaId
LEFT JOIN TblDepartments DEP ON E.DepartmentId = DEP.DepartmentId
LEFT JOIN TblDesignations DES ON E.DesignationId = DES.DesignationId
    END

    -- INSERT EMPLOYEE
    IF(@Flag = 'InsertEmployee')
    BEGIN
        INSERT INTO TblEmployees
        (
            Name,
            Email,
            PhoneNumber,
            DOB,
            Address,
			CityId,
			DistrictId,
            TalukaId,
            DepartmentId,
            DesignationId,
            Pincode,
            FilePath,
            CreatedDate,
            IsActive
        )
        VALUES
        (
            @Name,
            @Email,
            @PhoneNumber,
            @DOB,
            @Address,
			@CityId,        
            @DistrictId, 
            @TalukaId,
            @DepartmentId,
            @DesignationId,
            @Pincode,
            @FilePath,
            GETDATE(),
            @IsActive
        )
    END

    -- UPDATE EMPLOYEE
    IF(@Flag = 'UpdateEmployee')
    BEGIN
        UPDATE TblEmployees
        SET 
            Name = @Name,
            Email = @Email,
            PhoneNumber = @PhoneNumber,
            DOB = @DOB,
            Address = @Address,
			cityId = @CityId,
			DistrictId = @DistrictId,
            TalukaId = @TalukaId,
            DepartmentId = @DepartmentId,
            DesignationId = @DesignationId,
            Pincode = @Pincode,
            FilePath = @FilePath,
            UpdatedDate = GETDATE(),
            IsActive = @IsActive
        WHERE Id = @Id
    END

    -- DELETE EMPLOYEE
    IF(@Flag = 'DeleteEmployee')
    BEGIN
        DELETE FROM TblEmployees WHERE Id = @Id
    END

    -- FETCH EMPLOYEE BY ID (FOR EDIT)
    IF(@Flag = 'FetchEmployeeById')
    BEGIN
        SELECT 
    E.Id,
    E.Name,
    E.Email,
    E.PhoneNumber,
    E.DOB,
    E.Address,
    E.CityId,
    E.DistrictId,
    E.TalukaId,
    E.DepartmentId,
    E.DesignationId,
    E.Pincode,
    E.FilePath,
    E.IsActive
FROM TblEmployees E
WHERE E.Id = @Id
    END

    -- FETCH DESIGNATIONS
    IF(@Flag = 'FetchDesignation')
    BEGIN
        SELECT DesignationId, DesignationName FROM TblDesignations
    END

    -- FETCH DEPARTMENTS
    IF(@Flag = 'FetchDepartments')
    BEGIN
        SELECT DepartmentId, DepartmentName FROM TblDepartments
    END

    -- FETCH CITIES
    IF(@Flag = 'FetchCities')
    BEGIN
        SELECT CityId, CityName FROM TblCities
    END

    -- FETCH TALUKAS BY DISTRICT
    IF(@Flag = 'FetchTalukaByDistrictId')
    BEGIN
        SELECT TalukaId, TalukaName
        FROM TblTalukas
        WHERE DistrictId = @DistrictId -- Or update to correct parent key if needed
    END

    -- FETCH DISTRICTS BY CITY
    IF(@Flag = 'FetchDistrictByCityId')
    BEGIN
        SELECT DistrictId, DistrictName
        FROM TblDistricts
        WHERE CityId = @CityId
    END

END