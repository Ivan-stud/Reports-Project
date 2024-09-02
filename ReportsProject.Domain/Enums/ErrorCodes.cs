namespace ReportsProject.Domain.Enums;

public enum ErrorCodes : int
{
	InternalServerError = 0,

	ReportsNotFound = 10,
	ReportNotFound = 11,
	ReportAlreadyExists = 12,

	UserNotFound = 20,
	PasswordNotEqualsPasswordConfirm = 21,
	PasswordIsWrong = 22,
	UserAlreadyExists = 23,
	UserUnauthirizedAccess = 24,

	InvalidClientRequest = 30
}
