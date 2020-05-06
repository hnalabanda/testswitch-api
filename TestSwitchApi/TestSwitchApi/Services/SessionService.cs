﻿using System;
using Microsoft.AspNetCore.Http;
using TestSwitchApi.Repositories;

namespace TestSwitchApi.Services
{
    public class SessionService : ISessionService
    {
        private readonly IAdminRepo _adminRepo;

        public SessionService(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public bool RequestHasValidSessionId(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Session-Id"))
            {
                return false;
            }

            var sessionId = context.Request.Headers["Session-Id"];
            var session = _adminRepo.GetSession(sessionId);
            if (session == null)
            {
                return false;
            }

            if (!SessionInDate(session.SessionEnd))
            {
                return false;
            }

            return true;
        }

        private bool SessionInDate(DateTime SessionEndTime)
        {
            return SessionEndTime > DateTime.Now ? true : false;
        }
    }
}