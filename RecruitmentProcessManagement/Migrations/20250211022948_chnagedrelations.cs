using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentProcessManagement.Migrations
{
    /// <inheritdoc />
    public partial class chnagedrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateReviews_AspNetUsers_ReviewerID",
                table: "CandidateReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateStatusHistories_AspNetUsers_ChangedBy",
                table: "CandidateStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVerifications_AspNetUsers_VerifiedBy",
                table: "DocumentVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedbacks_AspNetUsers_InterviewerID",
                table: "InterviewFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "YearsOfExperience",
                table: "Positions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "InterviewerID",
                table: "Interviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Rating",
                table: "InterviewFeedbacks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "YearsOfExperience",
                table: "InterviewerSkills",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExperienceYears",
                table: "Interviewers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateReviews_AspNetUsers_ReviewerID",
                table: "CandidateReviews",
                column: "ReviewerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateStatusHistories_AspNetUsers_ChangedBy",
                table: "CandidateStatusHistories",
                column: "ChangedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVerifications_AspNetUsers_VerifiedBy",
                table: "DocumentVerifications",
                column: "VerifiedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedbacks_AspNetUsers_InterviewerID",
                table: "InterviewFeedbacks",
                column: "InterviewerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateReviews_AspNetUsers_ReviewerID",
                table: "CandidateReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateStatusHistories_AspNetUsers_ChangedBy",
                table: "CandidateStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVerifications_AspNetUsers_VerifiedBy",
                table: "DocumentVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedbacks_AspNetUsers_InterviewerID",
                table: "InterviewFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "InterviewerID",
                table: "Interviews");

            migrationBuilder.AlterColumn<string>(
                name: "YearsOfExperience",
                table: "Positions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "InterviewFeedbacks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "YearsOfExperience",
                table: "InterviewerSkills",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExperienceYears",
                table: "Interviewers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateReviews_AspNetUsers_ReviewerID",
                table: "CandidateReviews",
                column: "ReviewerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateStatusHistories_AspNetUsers_ChangedBy",
                table: "CandidateStatusHistories",
                column: "ChangedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVerifications_AspNetUsers_VerifiedBy",
                table: "DocumentVerifications",
                column: "VerifiedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedbacks_AspNetUsers_InterviewerID",
                table: "InterviewFeedbacks",
                column: "InterviewerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
