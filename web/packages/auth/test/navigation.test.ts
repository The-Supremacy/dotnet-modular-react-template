import { describe, expect, it } from "vitest";

import {
  loginPath,
  logoutPath,
  startLogin,
  startLogout,
  type BrowserLocation,
} from "../src";

describe("session navigation helpers", () => {
  it("starts login through the Host login route", () => {
    const assignedUrls: string[] = [];
    const location: BrowserLocation = {
      href: "http://localhost:5173/current?tab=me",
      assign(url) {
        assignedUrls.push(url);
      },
    };

    startLogin(location);

    expect(assignedUrls).toEqual([
      `${loginPath}?returnUrl=http%3A%2F%2Flocalhost%3A5173%2Fcurrent%3Ftab%3Dme`,
    ]);
  });

  it("starts logout through the Host logout route", () => {
    const assignedUrls: string[] = [];
    const location: BrowserLocation = {
      href: "http://localhost:5173/current",
      assign(url) {
        assignedUrls.push(url);
      },
    };

    startLogout(location);

    expect(assignedUrls).toEqual([logoutPath]);
  });
});
